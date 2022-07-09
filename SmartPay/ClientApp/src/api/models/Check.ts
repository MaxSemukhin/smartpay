/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Product } from './Product';
import type { User } from './User';

export type Check = {
    uid?: number;
    id?: number;
    user?: User;
    userId?: number;
    products?: Array<Product> | null;
};