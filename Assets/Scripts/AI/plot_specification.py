from networkx import DiGraph, draw, draw_networkx_edge_labels
from networkx.drawing.nx_agraph import graphviz_layout
import matplotlib
matplotlib.use('Agg')
import matplotlib.pyplot as plt
import sys
import yaml
import os

#TODO files including other files

def build_tree(root_name):
    if not G.has_node(root_name):
        G.add_node(root_name)
        node = nodes[root_name]

        if type(node) is str:
            node = {'action': node}

        if True in node:
            for n in node[True].split(';'):
                n = n.strip()
                build_tree(n)
                G.add_edge(root_name, n)
                labels[(root_name, n)] = 'yes'
        if False in node:
            for n in node[False].split(';'):
                n = n.strip()
                build_tree(n)
                G.add_edge(root_name, n)
                labels[(root_name, n)] = 'no'


def main():
    with open(sys.argv[1]) as file:
        spec = yaml.safe_load(file)
        name = os.path.splitext(os.path.basename(file.name))[0].capitalize() + 'Tree'

    global nodes
    nodes = spec['nodes']

    global labels
    labels = {}

    global G
    G = DiGraph()
    build_tree('ROOT')

    pos = graphviz_layout(G, prog='dot')
    draw(G, pos, with_labels=True, arrows=True, node_size=0,
         width=1, node_color='#333333', edge_color='#f85674')
    draw_networkx_edge_labels(G, pos, edge_labels=labels)
    
    plt.savefig(name + '.png', dpi=300)



if __name__ == "__main__":
    main()
