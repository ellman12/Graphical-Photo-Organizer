import { StoreApi, UseBoundStore } from "zustand";

type WithSelectors<S> = S extends { getState: () => infer T } ? S & { use: { [K in keyof T]: () => T[K] } } : never;

//https://zustand.docs.pmnd.rs/guides/auto-generating-selectors
export const createSelectors = <S extends UseBoundStore<StoreApi<object>>>(_store: S) => {
    const store = _store as WithSelectors<typeof _store>;
    store.use = {};
    for (const k of Object.keys(store.getState())) {
        //@ts-expect-error Don't need to know this type.
        (store.use as never)[k] = () => store((s) => s[k as keyof typeof s]);
    }

    return store;
};
