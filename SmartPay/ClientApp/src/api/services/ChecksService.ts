/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CheckViewModel } from '../models/CheckViewModel';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class ChecksService {

    /**
     * @returns CheckViewModel Success
     * @throws ApiError
     */
    public static getApiChecks(): CancelablePromise<Array<CheckViewModel>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/checks',
        });
    }

}