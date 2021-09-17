import { Component, OnInit } from '@angular/core';

import { API } from 'src/app/configs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Health Checker';

  ngOnInit() {
    console.log(`Health-Checker Application is running on port: ${API.EXTERNAL_HOST}`);
  }
}
