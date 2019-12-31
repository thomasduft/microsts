import { Component, Input, HostBinding } from '@angular/core';

@Component({
  selector: 'wcs-expander',
  templateUrl: './expander.component.html',
  styleUrls: ['./expander.component.scss']
})
export class ExpanderComponent {
  @HostBinding('class')
  public style = 'expander';

  @Input()
  public open = false;
}
