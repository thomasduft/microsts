import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SecretComponent } from './secret/secret.component';

@NgModule({
  declarations: [
    SecretComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    SecretComponent
  ]
})
export class SecretModule { }
