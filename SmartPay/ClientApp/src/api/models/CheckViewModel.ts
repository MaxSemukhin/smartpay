/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { CheckProduct } from './CheckProduct';

export type CheckViewModel = {
    uid?: number;
    id?: number;
    products?: Array<CheckProduct> | null;
};