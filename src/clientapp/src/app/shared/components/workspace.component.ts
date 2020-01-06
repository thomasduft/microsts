import { Component, HostBinding } from '@angular/core';

@Component({
  selector: 'tw-workspace',
  template: `
  <router-outlet></router-outlet>
 `
})
export class WorkspaceComponent {
  @HostBinding('class')
  public style = 'workspace';
}
