/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { MerchantCategory } from './MerchantCategory';
import type { Product } from './Product';
import type { User } from './User';

export type Merchant = {
    id?: number;
    category?: MerchantCategory;
    products?: Array<Product> | null;
    users?: Array<User> | null;
    name?: string | null;
};