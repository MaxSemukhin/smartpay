/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { SubCategoryViewModel } from './SubCategoryViewModel';

export type CategoryViewModel = {
    id?: number;
    mcc?: number;
    name?: string | null;
    subCategories?: Array<SubCategoryViewModel> | null;
};