import sys
import yaml
import re
import os

funcs = {
    'abs\(': 'Math.Abs(',
    'min\(': 'Math.Min(',
    'max\(': 'Math.Max(',
}

# replaces expressions' and functions
def parse_code(line):
    #expressions
    for key, value in EXPRESSIONS.items():
        line = re.sub(key, str(value), line)

    #functions
    for name, repl in funcs.items():
        line = re.sub(name, repl, line)

    if ';' in line: # multiple instructions
        return '{' + line + '}'
    else:
        return line


def create_tree(name):
    global CREATED_NODES
    if name not in CREATED_NODES:
        code_lines = []
        node = NODES[name]

        # node with just the action
        if type(node) is str:
            node = {'action': node}

        #left children
        if True in node:
            yes_nodes = [x.strip() for x in node[True].split(';')]
            for yes_node in yes_nodes:
                code_lines += create_tree(yes_node)
            left = 'new List<DecisionTree> {' + ', '.join(yes_nodes) + '}'
        else:
            left = 'null'

        # right children
        if False in node:
            no_nodes = [x.strip() for x in node[False].split(';')]
            for no_node in no_nodes:
                code_lines += create_tree(no_node)
            right = 'new List<DecisionTree> {' + ', '.join(no_nodes) + '}'
        else:
            right = 'null'


        #action
        if 'action' in node:
            action = f'(state, data) => {parse_code(node["action"])}'
        else:
            action = 'null'

        #decision
        if 'decision' in node:
            decision = f'(state, data) => {parse_code(node["decision"])}'
        else:
            decision = 'null'

        #comment
        if 'info' in node:
            info = f'// {node["info"]}\n\t\t'
        else:
            info = ''


        # add to created nodes
        CREATED_NODES.add(name)

        code_lines.append(f'{info}DecisionTree {name} = new DecisionTree("{name}", {action}, {decision}, {left}, {right});\n')
        return code_lines
    
    else:
        return []


def process_include(filenames):
    for filename in filenames:
        with open(filename) as file:
            spec = yaml.safe_load(file)
            exps = spec['expressions'] if 'expressions' in spec else {}
            nds = spec['nodes'] if 'nodes' in spec else {}
            incs = spec['include'] if 'include' in spec else []
            # expressions
            for name, exp in exps.items():
                if name not in EXPRESSIONS:
                    EXPRESSIONS[name] = exp
            # nodes
            for name, node in nds.items():
                if name not in NODES:
                    NODES[name] = node
            
            #other includes
            process_include([FOLDER_PATH + inc for inc in incs])


def main():
    with open(sys.argv[1]) as file:
        spec = yaml.safe_load(file)
        global FOLDER_PATH
        FOLDER_PATH = os.path.dirname(file.name) + '/'
        name = os.path.splitext(os.path.basename(file.name))[0].capitalize() + 'Tree'

    global EXPRESSIONS
    EXPRESSIONS = spec['expressions'] if 'expressions' in spec else {}

    global NODES
    NODES = spec['nodes']

    include = spec['include'] if 'include' in spec else {}
    process_include([FOLDER_PATH + i for i in include])

    global CREATED_NODES
    CREATED_NODES = set()

    tree_code = '\n\t\t'.join(create_tree('ROOT'))

    code = f'''
using System;
using System.Collections.Generic;

class {name} {{

    public static DecisionTree CreateTree() {{
        {tree_code}
        return ROOT;
    }}
}}
'''

    if not os.path.exists('generated_code'):
        os.mkdir('generated_code')

    out_path = 'generated_code/' + name + '.cs'
    with open(out_path, 'w') as file:
        file.write(code)


if __name__ == "__main__":
    main()
