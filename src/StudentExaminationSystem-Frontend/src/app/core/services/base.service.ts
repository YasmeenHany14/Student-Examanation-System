import { inject, Injectable } from '@angular/core';
import { routes } from '../constants/routs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { PagedListModel } from '../models/common/paged-list.model';
import { BaseResourceParametersModel } from '../models/common/base-resource-parameters.model';
import { BaseRequestModel, BaseResponseModel } from '../models/common/base-model';

@Injectable({
  providedIn: 'root'
})
export abstract class BaseService {
  protected readonly API_URL = routes.baseUrl;
  protected readonly httpClient = inject(HttpClient);

  protected abstract route: string;

  getAllPaged<TResourceParams extends BaseResourceParametersModel,
    TResponseModel extends BaseResponseModel>(queryParams: TResourceParams) {
    const params = new HttpParams({ fromObject: { ...queryParams } });
    return this.httpClient.get<PagedListModel<TResponseModel>>(this.API_URL + this.route, { params });
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
