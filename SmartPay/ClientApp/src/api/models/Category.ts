/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { SubCategory } from './SubCategory';

export type Category = {
    id?: number;
    mcc?: number;
    name?: string | null;
    subCategories?: Array<SubCategory> | null;
};