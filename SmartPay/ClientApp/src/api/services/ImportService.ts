/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class ImportService {

    /**
     * @param formData 
     * @returns any Success
     * @throws ApiError
     */
    public static postApiImportImport(
formData?: {
data?: Blob;
},
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/import/import',
            formData: formData,
            mediaType: 'multipart/form-data',
        });
    }

}