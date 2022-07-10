/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Check } from './Check';
import type { Product } from './Product';

export type CheckProduct = {
    checkUid?: number;
    check?: Check;
    productId?: number;
    product?: Product;
    cost?: number;
    amount?: number;
};