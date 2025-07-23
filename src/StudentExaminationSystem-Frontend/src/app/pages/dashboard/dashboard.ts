import {Component, OnInit, signal} from '@angular/core';
import { DashboardService } from '../../core/services/dashboard.service';
import { AdminDashboardResponse } from '../../core/models/dashboard.model';
import {ProgressSpinner} from 'primeng/progressspinner';
import {Card} from 'primeng/card';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.html',
  imports: [
    ProgressSpinner,
    Card
  ],
  styleUrls: ['./dashboard.scss']
})
export class Dashboard implements OnInit {
  dashboardData = signal<AdminDashboardResponse | null>(null);
  loading = signal(true);
  error = signal<string | null>(null);

  constructor(private dashboardService: DashboardService) {}

  ngOnInit() {
    this.dashboardService.getAdminDashboardData().subscribe({
      next: (data) => {
        this.dashboardData.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load dashboard data.');
        this.loading.set(false);
      }
    });
  }
}
