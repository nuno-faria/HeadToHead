import sys
import yaml
import re
import os

funcs = {
    'abs\(': 'Math.Abs(',
}

# replaces constants name by their value and subs functions by their respective name
def parse_code(line):
    #constants
    for key, value in constants.items():
        line = re.sub(key, str(value), line)

    #functions
    for name, repl in funcs.items():
        line = re.sub(name, repl, line)

    if ';' in line: # multiple instructions
        return '{' + line + '}'
    else:
        return line


def create_tree(name):
    global created_nodes
    if name not in created_nodes:
        code_lines = []
        node = nodes[name]

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

        # add to created nodes
        created_nodes.add(name)

        code_lines.append(f'DecisionTree {name} = new DecisionTree("{name}", {action}, {decision}, {left}, {right});')
        return code_lines
    
    else:
        return []


def main():
    with open(sys.argv[1]) as file:
        spec = yaml.safe_load(file)
        name = os.path.splitext(os.path.basename(file.name))[0].capitalize() + 'Tree'

    global constants
    constants = spec['expressions']

    global nodes
    nodes = spec['nodes']

    global created_nodes
    created_nodes = set()

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
