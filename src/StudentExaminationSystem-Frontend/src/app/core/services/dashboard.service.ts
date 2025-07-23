import {inject, Injectable} from '@angular/core';
import {routes} from '../constants/routs';
import {HttpClient} from '@angular/common/http';
import {AdminDashboardResponse} from '../models/dashboard.model';

@Injectable({
  providedIn: 'root'
})

export class DashboardService {
  private readonly API_URL = routes.baseUrl;
  private readonly httpClient = inject(HttpClient);

  getAdminDashboardData() {
    return this.httpClient.get<AdminDashboardResponse>(this.API_URL + routes.dashboard);
  }
}
