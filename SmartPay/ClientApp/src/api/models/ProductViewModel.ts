/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { MerchantViewModel } from './MerchantViewModel';
import type { SubCategoryProductViewModel } from './SubCategoryProductViewModel';

export type ProductViewModel = {
    id?: number;
    name?: string | null;
    price?: number;
    merchant?: MerchantViewModel;
    category?: SubCategoryProductViewModel;
};