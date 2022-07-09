/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Product } from './Product';

export type CheckViewModel = {
    uid?: number;
    id?: number;
    products?: Array<Product> | null;
};