import {
  Component,
  ContentChildren,
  QueryList,
  AfterContentInit,
  HostBinding
} from '@angular/core';

import { TabComponent } from './tab.component';

@Component({
  selector: 'wcs-tabs',
  templateUrl: './tabs.component.html',
  styleUrls: ['./tabs.component.scss']
})
export class TabsComponent implements AfterContentInit {
  @HostBinding('class')
  public style = 'tabs';

  @ContentChildren(TabComponent)
  public tabs: QueryList<TabComponent>;

  public ngAfterContentInit(): void {
    const activeTabs = this.tabs.filter((tab) => tab.active);
    if (activeTabs.length === 0) {
      setTimeout(() => this.selectTab(this.tabs.first));
    }
  }

  public getTabIndex(tab: TabComponent): number | null {
    return !tab.active ? 0 : null;
  }

  public selectTab(tab: TabComponent) {
    this.tabs.toArray().forEach(t => t.active = false);

    tab.active = true;
  }

  public keydownOnTab(event: KeyboardEvent, tab: TabComponent) {
    // enter and spacebar
    if (event.keyCode === 13 || event.keyCode === 32) {
      this.selectTab(tab);
    }
  }
}
