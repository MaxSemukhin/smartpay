/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Merchant } from './Merchant';

export type MerchantCategory = {
    id?: number;
    name?: string | null;
    merchants?: Array<Merchant> | null;
};