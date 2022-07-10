/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { CheckProduct } from './CheckProduct';
import type { Merchant } from './Merchant';
import type { SubCategory } from './SubCategory';

export type Product = {
    id?: number;
    name?: string | null;
    price?: number;
    checks?: Array<CheckProduct> | null;
    merchantId?: number;
    merchant?: Merchant;
    categoryId?: number;
    category?: SubCategory;
};