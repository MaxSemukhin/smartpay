/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { MerchantCategory } from './MerchantCategory';
import type { Product } from './Product';

export type Merchant = {
    id?: number;
    category?: MerchantCategory;
    products?: Array<Product> | null;
    name?: string | null;
};