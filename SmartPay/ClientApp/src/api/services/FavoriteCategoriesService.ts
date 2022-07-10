/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CategoryViewModel } from '../models/CategoryViewModel';
import type { FavoriteCategoryPost } from '../models/FavoriteCategoryPost';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class FavoriteCategoriesService {

    /**
     * @returns CategoryViewModel Success
     * @throws ApiError
     */
    public static getApiFavoritecategories(): CancelablePromise<Array<CategoryViewModel>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/favoritecategories',
        });
    }

    /**
     * @param requestBody 
     * @returns CategoryViewModel Success
     * @throws ApiError
     */
    public static postApiFavoritecategories(
requestBody?: Array<FavoriteCategoryPost>,
): CancelablePromise<Array<CategoryViewModel>> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/favoritecategories',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }

    /**
     * @returns CategoryViewModel Success
     * @throws ApiError
     */
    public static getApiFavoritecategoriesAll(): CancelablePromise<Array<CategoryViewModel>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/favoritecategories/all',
        });
    }

}