/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { JwtData } from '../models/JwtData';
import type { LoginViaIdModel } from '../models/LoginViaIdModel';
import type { User } from '../models/User';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class AuthService {

    /**
     * @param requestBody 
     * @returns JwtData Success
     * @throws ApiError
     */
    public static postApiAuthLoginId(
requestBody?: LoginViaIdModel,
): CancelablePromise<JwtData> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/auth/login/id',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }

    /**
     * @returns User Success
     * @throws ApiError
     */
    public static getApiAuthMe(): CancelablePromise<User> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/auth/me',
        });
    }

}