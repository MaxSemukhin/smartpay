/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { SubCategory } from './SubCategory';
import type { User } from './User';

export type Category = {
    id?: number;
    mcc?: number;
    name?: string | null;
    users?: Array<User> | null;
    subCategories?: Array<SubCategory> | null;
};