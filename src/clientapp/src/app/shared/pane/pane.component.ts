import { Component, HostBinding } from '@angular/core';

@Component({
  selector: 'tw-pane',
  templateUrl: './pane.component.html',
  styleUrls: ['./pane.component.less']
})
export class PaneComponent {
  @HostBinding('class')
  public style = 'pane';
}
