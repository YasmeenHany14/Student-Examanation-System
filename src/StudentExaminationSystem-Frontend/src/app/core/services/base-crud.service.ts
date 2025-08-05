import { Injectable } from '@angular/core';
import { HttpHeaders, HttpParams } from '@angular/common/http';
import { PagedListModel } from '../models/common/paged-list.model';
import { BaseResourceParametersModel } from '../models/resource-parameters/base-resource-parameters.model';
import { BaseRequestModel, BaseResponseModel } from '../models/common/base-model';
import {BaseService} from './base.service';

@Injectable({
  providedIn: 'root'
})
export abstract class BaseCrudService extends BaseService {
  getAllPaged<TResourceParams extends BaseResourceParametersModel,
    TResponseModel extends BaseResponseModel>(queryParams: TResourceParams) {
    const filteredParams: { [key: string]: string | number | boolean | readonly (string | number | boolean)[] } = {};
    Object.keys(queryParams).forEach(key => {
      const value = queryParams[key];
      if (value !== null) {
        filteredParams[key] = value;
      }
    });

    const params = new HttpParams({ fromObject: filteredParams });
    return this.httpClient.get<PagedListModel<TResponseModel>>(this.API_URL + this.route, { params });
  }

  getById<TResponseModel extends BaseResponseModel>(id: number) {
    return this.httpClient.get<TResponseModel>(this.API_URL + this.route + '/' + id);
  }

  getByGuid<TResponseModel extends BaseResponseModel>(id: string) {
    return this.httpClient.get<TResponseModel>(this.API_URL + this.route + '/' + id);
  }

  createModel<TRequestModel extends BaseRequestModel>(model: TRequestModel) {
    return this.httpClient.post(this.API_URL + this.route, model);
  }

  deleteModel(id: number) {
    return this.httpClient.delete(this.API_URL + this.route + '/' + id);
  }

  updateModel(id: number, patchDoc: any) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json-patch+json',
      'Accept': 'text/plain'
    });
    return this.httpClient.patch(this.API_URL + this.route + '/' + id, patchDoc, { headers });
  }
}
