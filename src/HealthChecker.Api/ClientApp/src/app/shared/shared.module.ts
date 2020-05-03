import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './material';

@NgModule({
  imports: [
    MaterialModule,
  ],
  exports: [
    CommonModule,

    MaterialModule,
  ]
})
export class SharedModule { }
