#!/usr/bin/env python3

import argparse
import csv

parser = argparse.ArgumentParser(description='Compare benchmark results.')
parser.add_argument('baseline', metavar='baseline', type=str, nargs=1, help='baseline')
parser.add_argument('new', metavar='new', type=str, nargs=1, help='name')
args = parser.parse_args()


OUTPUT_LOG = 'Output.log'


def read_and_parse_csv(f) -> dict[str, int]:
    res = dict()
    results = list()
    for l in f:
        if l.startswith('Single-thread results') or l.startswith('All results'):
            results = [f.readline().strip().replace(';', ',', -1), f.readline().strip().replace(';', ',', -1)]

    if len(results) > 0:
        res = next(csv.DictReader(results))

    return res


def parse():
    print(f'Will compare {args.new} against {args.baseline}')
    print(f'baseline: {args.baseline}\nnew: {args.new}')
    with open(args.baseline[0] + '/' + OUTPUT_LOG, 'r') as f:
        baseline = read_and_parse_csv(f)

    with open(args.new[0] + '/' + OUTPUT_LOG, 'r') as f:
        new = read_and_parse_csv(f)

    for k, v in baseline.items():
        try:
            v_n = new[k]
            v_f = float(v)
            v_n_f = float(v_n)
            diff = v_n_f * 100 / v_f
            print(f'{k}: {diff:.2f}%')
        except ValueError:
            print(f'{k}: "{v}", "{v_n}"')
        except KeyError:
            pass
        except TypeError:
            pass
        except ZeroDivisionError:
            print(f'{k}: Inf %')


if __name__ == '__main__':
    parse()

