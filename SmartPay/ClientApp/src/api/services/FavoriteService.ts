/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CategoryViewModel } from '../models/CategoryViewModel';
import type { CategoryWithMerhands } from '../models/CategoryWithMerhands';
import type { FavoriteCategoryPost } from '../models/FavoriteCategoryPost';
import type { MerchantViewModel } from '../models/MerchantViewModel';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class FavoriteService {

    /**
     * @returns CategoryViewModel Success
     * @throws ApiError
     */
    public static getApiFavoriteCategories(): CancelablePromise<Array<CategoryViewModel>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/favorite/categories',
        });
    }

    /**
     * @param requestBody 
     * @returns CategoryViewModel Success
     * @throws ApiError
     */
    public static postApiFavoriteCategories(
requestBody?: Array<FavoriteCategoryPost>,
): CancelablePromise<Array<CategoryViewModel>> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/favorite/categories',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }

    /**
     * @returns MerchantViewModel Success
     * @throws ApiError
     */
    public static getApiFavoriteMerchants(): CancelablePromise<Array<MerchantViewModel>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/favorite/merchants',
        });
    }

    /**
     * @param requestBody 
     * @returns CategoryViewModel Success
     * @throws ApiError
     */
    public static postApiFavoriteMerchants(
requestBody?: Array<MerchantViewModel>,
): CancelablePromise<Array<CategoryViewModel>> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/favorite/merchants',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }

    /**
     * @returns CategoryViewModel Success
     * @throws ApiError
     */
    public static getApiFavoriteCategoriesAll(): CancelablePromise<Array<CategoryViewModel>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/favorite/categories/all',
        });
    }

    /**
     * @returns CategoryWithMerhands Success
     * @throws ApiError
     */
    public static getApiFavoriteMerchantsAll(): CancelablePromise<Array<CategoryWithMerhands>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/favorite/merchants/all',
        });
    }

}