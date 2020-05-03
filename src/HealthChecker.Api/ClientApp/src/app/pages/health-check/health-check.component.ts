import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SignalRService } from 'src/app/core';

import { API } from 'src/app/configs';
import { IHealthCheckModel } from 'src/app/core';
import { Observable } from 'rxjs';



@Component({
  selector: 'app-health-check',
  templateUrl: './health-check.component.html',
  styleUrls: ['./health-check.component.css']
})
export class HealthCheckComponent implements OnInit {
  public westState$: Observable<IHealthCheckModel>;
  public eastState$: Observable<IHealthCheckModel>;
  public southState$: Observable<IHealthCheckModel>;

  constructor(private signalRService: SignalRService, private http: HttpClient) { }

  ngOnInit() {
    this.signalRService.startWestConnection();
    this.signalRService.startEastConnection();
    this.signalRService.startSouthConnection();

    this.signalRService.addTransferDataListeners();   

    this.startWestJob();
    this.startEastJob();
    this.startSouthJob();

    this.westState$ = this.signalRService.GetWestStateObservable;
    this.eastState$ = this.signalRService.GetEastStateObservable;
    this.southState$ = this.signalRService.GetSouthStateObservable;
  }
 
  private startWestJob = () => {
    this.http.get<IHealthCheckModel>(API.WEST)
    .subscribe();
  }

  private startEastJob = () => {
    this.http.get<IHealthCheckModel>(API.EAST)
    .subscribe();
  }

  private startSouthJob = () => {
    this.http.get<IHealthCheckModel>(API.SOUTH)
    .subscribe();
  }

}
