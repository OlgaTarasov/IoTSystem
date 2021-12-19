import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { interval } from 'rxjs';
import { Time } from '@angular/common';

@Component({
  selector: 'app-signal-data',
  templateUrl: './signal-data.component.html'
})
export class SignalDataComponent {
  public anomalySignals: Signal[] = [];
  public sineSignals: Signal[] = [];
  public stateSignals: Signal[] = [];
  public page: number = 1;
  public pageSize: number = 100;
  public anomalyPage: number = 1;
  public anomalyPageSize: number = 100;
  public updateTime: any = Date.now();
  public updateTime_Anomaly: any = Date.now();

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    interval(1000).subscribe(x => {
      http.get<Signal[]>(baseUrl + 'signal/anomaly/page'+this.anomalyPage +'/size'+this.anomalyPageSize).subscribe(result => {
        this.anomalySignals = result;
        this.updateTime_Anomaly = Date.now();
      }, error => console.error(error));
      http.get<Signal[]>(baseUrl + 'signal/sine/page'+this.page +'/size'+this.pageSize).subscribe(result => {
        this.sineSignals = result;
        this.updateTime = Date.now();
      }, error => console.error(error));
      http.get<Signal[]>(baseUrl + 'signal/state/page'+this.page +'/size'+this.pageSize).subscribe(result => {
        this.stateSignals = result;
      }, error => console.error(error));
    });
  }

}

interface Signal {
  id: number;
  value: number;
  timeStamp: string;
  isAnomaly: number;
  source: string;
}

interface SignalSource {
  id: number;
  code: number;
  source: string;
}
