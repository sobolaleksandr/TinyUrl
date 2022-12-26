import http from "../http-common";
import TinyUrl from "../types/TinyUrl";

const get = (id: any) => {
  return http.get<TinyUrl>(`/Url/${id}`);
};

const create = (data: TinyUrl) => {
  return http.post<TinyUrl>("/Url", data);
};

const TinyUrlService = {
  get,
  create
};

export default TinyUrlService;
