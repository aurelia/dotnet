import { HttpClient } from 'aurelia-fetch-client';
import { autoinject } from 'aurelia-framework';
import { Forecast } from './forecast';


@autoinject
export class Samples {
  forecasts: Forecast[];
  constructor(private http: HttpClient) {
    http.configure(config => {
      config
        .useStandardConfiguration()
        .withBaseUrl('/api/sampledata/');
    });
  }


  getAlertType(temperature: number) {
    if (temperature < 32) { return 'warning'; }
    if (temperature > 32 && temperature < 70) { return 'success'; }
    if (temperature > 70) { return 'danger'; }
  }

  async activate() {
    this.forecasts = await this.http.fetch('weatherforecasts').then(y => y.json());    
  }
}
