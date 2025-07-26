import {inject, Injectable} from '@angular/core';
import {routes} from '../constants/routs';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export abstract class BaseService {
  protected readonly API_URL = routes.baseUrl;
  protected readonly httpClient = inject(HttpClient);

  protected abstract route: string;
}
