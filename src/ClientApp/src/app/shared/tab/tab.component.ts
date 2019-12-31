import { Component, Input, HostBinding } from '@angular/core';

@Component({
  selector: 'wcs-tab',
  templateUrl: './tab.component.html',
  styleUrls: ['./tab.component.scss']
})
export class TabComponent {
  @HostBinding('class')
  public style = 'tabs__tab';

  @Input()
  public title: string;

  @Input()
  public active = false;
}
