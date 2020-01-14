import { Injectable } from '@angular/core';

import { HttpWrapperService } from '../../shared';

import { CoreModule } from '../core.module';
import { ClientConfiguration } from '../models';

@Injectable({
  providedIn: CoreModule
})
export class ClientConfigProvider {
  public config: ClientConfiguration;

  public constructor(
    private http: HttpWrapperService
  ) { }

  public load(): Promise<any> {
    const clientId = 'spaclient';
    return this.http.get<ClientConfiguration>(`client/config/${clientId}`)
      .toPromise()
      .then((data: ClientConfiguration) => {
        this.config = data;
      });
  }
}

export function clientConfigProviderFactory(provider: ClientConfigProvider) {
  return () => provider.load();
}
