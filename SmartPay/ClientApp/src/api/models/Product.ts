/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Check } from './Check';
import type { Merchant } from './Merchant';
import type { SubCategory } from './SubCategory';

export type Product = {
    id?: number;
    name?: string | null;
    price?: number;
    checks?: Array<Check> | null;
    merchant?: Merchant;
    category?: SubCategory;
};