import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { HttpService } from './http.service';
import { Log } from '@modules/project/logging-terminal/logging-terminal.component';

@Injectable({
  providedIn: 'root'
})
export class ProjectLogsService {
  private logsHubConnection: HubConnection;
  routePrefix = '/logs';
  constructor(private httpService: HttpService) {
   }
   private logs$ = new Subject<Log[]>();
   private rawLogs$ = new Subject<string[]>();
   public buildConnection() {
     this.logsHubConnection = new HubConnectionBuilder()
     .withUrl(`${environment.signalRUrl}/logsHub`)
     .build();
   }

   public getLogsOfHistory(projectId: number, buildHisotryId: number): Observable<Log[]>{
      return this.httpService.getRequest(`${this.routePrefix}/${projectId}/${buildHisotryId}`);
   }

   sendRawLogs(logs){
    return this.rawLogs$.next(logs);
   }

   receiveRawLogs(){
    return this.rawLogs$.asObservable();
   }


   sendLogs(logs){
    return this.logs$.next(logs);
   }

   receiveLogs(){
    return this.logs$.asObservable();
   }

   public startConnectionAndJoinGroup(groupName: string) {
     this.logsHubConnection
      .start()
      .then(() => {
        console.log('Connection started...');
        this.logsHubConnection.invoke('JoinGroup', groupName); // Automatically join group after connecting
      })
      .catch(err => {
        console.log('Error while starting connection: ' + err);

        setTimeout(function() {
          this.startConnection();
        }, 3000);
      });
   }

   public stopConnection() {
     this.logsHubConnection
      .stop(); // It also makes user leave his group automaticlly after disconnecting
   }

   public logsListener(logSubject: Subject<string>) {
     this.logsHubConnection.on('Broadcast', (data) => {
       logSubject.next(data);
     });
   }
}
