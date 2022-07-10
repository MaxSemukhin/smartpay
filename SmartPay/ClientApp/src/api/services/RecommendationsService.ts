/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Recommendation } from '../models/Recommendation';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class RecommendationsService {

    /**
     * @returns Recommendation Success
     * @throws ApiError
     */
    public static getApiRecommendations(): CancelablePromise<Array<Recommendation>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/recommendations',
        });
    }

}