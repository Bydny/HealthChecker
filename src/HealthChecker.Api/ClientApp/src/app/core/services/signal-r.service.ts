import { Injectable } from '@angular/core';
import * as signalR from "@aspnet/signalr";

import { IHealthCheckModel } from '../models';
import { SIGNALR, HUB_EVENTS } from 'src/app/configs';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private westState$ = new BehaviorSubject<IHealthCheckModel>(null);
  private eastState$ = new BehaviorSubject<IHealthCheckModel>(null);
  private southState$ = new BehaviorSubject<IHealthCheckModel>(null);
 
  private westHubConnection: signalR.HubConnection;
  private eastHubConnection: signalR.HubConnection;
  private southHubConnection: signalR.HubConnection;
 
  public startWestConnection = () => {
    this.westHubConnection = new signalR.HubConnectionBuilder()
    .withUrl(SIGNALR.WEST)
    .build();
 
    this.westHubConnection
      .start()
      .then(() => console.log('West hub connection started'))
      .catch(err => console.log('Error while starting West hub connection: ' + err));
  }
 
  public startEastConnection = () => {
    this.eastHubConnection = new signalR.HubConnectionBuilder()
    .withUrl(SIGNALR.EAST)
    .build();
 
    this.eastHubConnection
      .start()
      .then(() => console.log('East hub connection started'))
      .catch(err => console.log('Error while starting East hub connection: ' + err));
  }

  public startSouthConnection = () => {
    this.southHubConnection = new signalR.HubConnectionBuilder()
    .withUrl(SIGNALR.SOUTH)
    .build();
 
    this.southHubConnection
      .start()
      .then(() => console.log('South hub connection started'))
      .catch(err => console.log('Error while starting South hub connection: ' + err));
  }

  public addTransferDataListeners = () => {
    this.westHubConnection.on(HUB_EVENTS.WEST, (data: IHealthCheckModel) => {
      this.westState$.next(data);
      console.log(data);
    });

    this.eastHubConnection.on(HUB_EVENTS.EAST, (data: IHealthCheckModel) => {
      this.eastState$.next(data);
      console.log(data);
    });

    this.southHubConnection.on(HUB_EVENTS.SOUTH, (data: IHealthCheckModel) => {
      this.southState$.next(data);
      console.log(data);
    });
  }

  public get GetWestStateObservable(): Observable<IHealthCheckModel> {
    return this.westState$.asObservable();
  }

  public get GetEastStateObservable(): Observable<IHealthCheckModel> {
    return this.eastState$.asObservable();
  }

  public get GetSouthStateObservable(): Observable<IHealthCheckModel> {
    return this.southState$.asObservable();
  }

}
