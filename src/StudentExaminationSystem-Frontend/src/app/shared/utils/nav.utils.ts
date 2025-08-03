import { ActivatedRoute, Router } from '@angular/router';

export function changeQueryParams(
  route: ActivatedRoute,
  router: Router,
  queryName: string,
  queryValue: string,
  op: 'Remove' | 'Update'
) {
  const currentParams = route.snapshot.queryParams;
  const newParams = { ...currentParams };

  if (op === 'Remove') {
    delete newParams[queryName];
  } else if (op === 'Update') {
    newParams[queryName] = queryValue;
  }

  router.navigate([], {
    relativeTo: route,
    queryParams: newParams,
    queryParamsHandling: 'merge'
  });
}

export function getFromQueryParams(route: ActivatedRoute, queryName: string): string | null {
  const currentParams = route.snapshot.queryParams;
  return currentParams[queryName] || null;
}
