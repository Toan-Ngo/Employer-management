import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { forkJoin, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class WorkForceOverviewService {

  api = 'https://localhost:44387/api/admin';

  constructor(private http: HttpClient) {}

  getDashboard(): Observable<any> {

    return forkJoin({
      employees: this.http.get(`${this.api}/employee`),

      departments: this.http.get(`${this.api}/department`),

      leaves: this.http.get(`${this.api}/leaverequest`),

      salaries: this.http.get(`${this.api}/salary`),

      attendance: this.http.get(`${this.api}/attendance`)
    });

  }

}