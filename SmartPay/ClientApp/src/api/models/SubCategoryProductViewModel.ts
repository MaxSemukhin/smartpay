/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { CategoryViewModel } from './CategoryViewModel';

export type SubCategoryProductViewModel = {
    id?: number;
    name?: string | null;
    imageUrl?: string | null;
    category?: CategoryViewModel;
};