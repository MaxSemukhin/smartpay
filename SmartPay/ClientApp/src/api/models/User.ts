/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Category } from './Category';
import type { Check } from './Check';
import type { Merchant } from './Merchant';

export type User = {
    id: number;
    checks?: Array<Check> | null;
    favoriteCategories?: Array<Category> | null;
    favoriteMerchants?: Array<Merchant> | null;
    isAdmin?: boolean;
    userName?: string | null;
    normalizedUserName?: string | null;
    email?: string | null;
    normalizedEmail?: string | null;
    emailConfirmed?: boolean;
    passwordHash?: string | null;
    securityStamp?: string | null;
    concurrencyStamp?: string | null;
    phoneNumber?: string | null;
    phoneNumberConfirmed?: boolean;
    twoFactorEnabled?: boolean;
    lockoutEnd?: string | null;
    lockoutEnabled?: boolean;
    accessFailedCount?: number;
};