/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { MerchantViewModel } from './MerchantViewModel';

export type CategoryWithMerhands = {
    id?: number;
    name?: string | null;
    merchants?: Array<MerchantViewModel> | null;
};