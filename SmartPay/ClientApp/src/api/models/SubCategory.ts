/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Category } from './Category';
import type { Product } from './Product';

export type SubCategory = {
    id?: number;
    name?: string | null;
    categoryId?: number;
    category?: Category;
    products?: Array<Product> | null;
};