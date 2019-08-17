from networkx import DiGraph, draw, draw_networkx_labels, draw_networkx_edge_labels
from networkx.drawing.nx_agraph import graphviz_layout
import matplotlib
matplotlib.use('Agg')
import matplotlib.pyplot as plt
import sys
import yaml
import os

YES_COLOR = '#5cf873'
NO_COLOR = '#f85c79'
YES_NO_COLOR = '#f5ec42'

def build_tree(root_name):
    if not G.has_node(root_name):
        node = NODES[root_name]
        G.add_node(root_name)

        if type(node) is str:
            node = {'action': node}

        if True in node:
            for n in node[True].split(';'):
                n = n.strip()
                build_tree(n)
                G.add_edge(root_name, n, color=YES_COLOR)
                edge_labels[(root_name, n)] = 'yes'

        if False in node:
            for n in node[False].split(';'):
                n = n.strip()
                build_tree(n)
                # edge in both yes and no
                if (G.has_edge(root_name, n)):
                    G.add_edge(root_name, n, color=YES_NO_COLOR)
                    edge_labels[(root_name, n)] = 'yes/no'
                else:
                    G.add_edge(root_name, n, color=NO_COLOR)
                    edge_labels[(root_name, n)] = 'no'

        if 'info' in node:
            node_labels[root_name] = '\n\n\n' + node['info']
        else:
            node_labels[root_name] = ''


def process_include(filenames):
    for filename in filenames:
        with open(filename) as file:
            spec = yaml.safe_load(file)
            nds = spec['nodes'] if 'nodes' in spec else {}
            incs = spec['include'] if 'include' in spec else []
            
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

    fig_width = float(sys.argv[2])
    fig_height = float(sys.argv[3])

    global NODES
    NODES = spec['nodes']

    include = spec['include'] if 'include' in spec else {}
    process_include([FOLDER_PATH + i for i in include])
    
    global edge_labels, node_labels
    edge_labels = {}
    node_labels = {}

    global G
    G = DiGraph()
    build_tree('ROOT')
    
    edges = G.edges()
    colors = [G[u][v]['color'] for u, v in edges]
    pos = graphviz_layout(G, prog='dot')

    plt.figure(figsize=(fig_width, fig_height))
    draw(G, pos, with_labels=True, arrows=True, node_size=0,
         width=1, node_color='#333333', edge_color=colors, font_size=12)
    draw_networkx_labels(G, pos, labels=node_labels, font_size=7, font_color='#666666')
    draw_networkx_edge_labels(G, pos, edge_labels=edge_labels, font_size=6)
    #x_values, y_values = zip(*pos.values())
    #x_max = max(x_values)
    #x_min = min(x_values)
    #x_margin = (x_max - x_min) * 0.15
    #plt.xlim(x_min - x_margin, x_max + x_margin)
    plt.savefig(name + '.png', dpi=300)



if __name__ == "__main__":
    main()
