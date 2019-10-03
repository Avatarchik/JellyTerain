using SimpleJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
	public class NetworkMatch : MonoBehaviour
	{
		public delegate void BasicResponseDelegate(bool success, string extendedInfo);

		public delegate void DataResponseDelegate<T>(bool success, string extendedInfo, T responseData);

		private delegate void InternalResponseDelegate<T, U>(T response, U userCallback);

		private Uri m_BaseUri = new Uri("https://mm.unet.unity3d.com");

		public Uri baseUri
		{
			get
			{
				return m_BaseUri;
			}
			set
			{
				m_BaseUri = value;
			}
		}

		[Obsolete("This function is not used any longer to interface with the matchmaker. Please set up your project by logging in through the editor connect dialog.", true)]
		public void SetProgramAppID(AppID programAppID)
		{
		}

		public Coroutine CreateMatch(string matchName, uint matchSize, bool matchAdvertise, string matchPassword, string publicClientAddress, string privateClientAddress, int eloScoreForMatch, int requestDomain, DataResponseDelegate<MatchInfo> callback)
		{
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				Debug.LogError("Matchmaking is not supported on WebGL player.");
				return null;
			}
			return CreateMatch(new CreateMatchRequest
			{
				name = matchName,
				size = matchSize,
				advertise = matchAdvertise,
				password = matchPassword,
				publicAddress = publicClientAddress,
				privateAddress = privateClientAddress,
				eloScore = eloScoreForMatch,
				domain = requestDomain
			}, callback);
		}

		internal Coroutine CreateMatch(CreateMatchRequest req, DataResponseDelegate<MatchInfo> callback)
		{
			if (callback == null)
			{
				Debug.Log("callback supplied is null, aborting CreateMatch Request.");
				return null;
			}
			Uri uri = new Uri(baseUri, "/json/reply/CreateMatchRequest");
			Debug.Log("MatchMakingClient Create :" + uri);
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("version", Request.currentVersion);
			wWWForm.AddField("projectId", Application.cloudProjectId);
			wWWForm.AddField("sourceId", Utility.GetSourceID().ToString());
			wWWForm.AddField("accessTokenString", 0);
			wWWForm.AddField("domain", req.domain);
			wWWForm.AddField("name", req.name);
			wWWForm.AddField("size", req.size.ToString());
			wWWForm.AddField("advertise", req.advertise.ToString());
			wWWForm.AddField("password", req.password);
			wWWForm.AddField("publicAddress", req.publicAddress);
			wWWForm.AddField("privateAddress", req.privateAddress);
			wWWForm.AddField("eloScore", req.eloScore.ToString());
			wWWForm.headers["Accept"] = "application/json";
			WWW client = new WWW(uri.ToString(), wWWForm);
			return StartCoroutine(ProcessMatchResponse<CreateMatchResponse, DataResponseDelegate<MatchInfo>>(client, OnMatchCreate, callback));
		}

		internal virtual void OnMatchCreate(CreateMatchResponse response, DataResponseDelegate<MatchInfo> userCallback)
		{
			if (response.success)
			{
				Utility.SetAccessTokenForNetwork(response.networkId, new NetworkAccessToken(response.accessTokenString));
			}
			userCallback(response.success, response.extendedInfo, new MatchInfo(response));
		}

		public Coroutine JoinMatch(NetworkID netId, string matchPassword, string publicClientAddress, string privateClientAddress, int eloScoreForClient, int requestDomain, DataResponseDelegate<MatchInfo> callback)
		{
			return JoinMatch(new JoinMatchRequest
			{
				networkId = netId,
				password = matchPassword,
				publicAddress = publicClientAddress,
				privateAddress = privateClientAddress,
				eloScore = eloScoreForClient,
				domain = requestDomain
			}, callback);
		}

		internal Coroutine JoinMatch(JoinMatchRequest req, DataResponseDelegate<MatchInfo> callback)
		{
			if (callback == null)
			{
				Debug.Log("callback supplied is null, aborting JoinMatch Request.");
				return null;
			}
			Uri uri = new Uri(baseUri, "/json/reply/JoinMatchRequest");
			Debug.Log("MatchMakingClient Join :" + uri);
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("version", Request.currentVersion);
			wWWForm.AddField("projectId", Application.cloudProjectId);
			wWWForm.AddField("sourceId", Utility.GetSourceID().ToString());
			wWWForm.AddField("accessTokenString", 0);
			wWWForm.AddField("domain", req.domain);
			wWWForm.AddField("networkId", req.networkId.ToString());
			wWWForm.AddField("password", req.password);
			wWWForm.AddField("publicAddress", req.publicAddress);
			wWWForm.AddField("privateAddress", req.privateAddress);
			wWWForm.AddField("eloScore", req.eloScore.ToString());
			wWWForm.headers["Accept"] = "application/json";
			WWW client = new WWW(uri.ToString(), wWWForm);
			return StartCoroutine(ProcessMatchResponse<JoinMatchResponse, DataResponseDelegate<MatchInfo>>(client, OnMatchJoined, callback));
		}

		internal void OnMatchJoined(JoinMatchResponse response, DataResponseDelegate<MatchInfo> userCallback)
		{
			if (response.success)
			{
				Utility.SetAccessTokenForNetwork(response.networkId, new NetworkAccessToken(response.accessTokenString));
			}
			userCallback(response.success, response.extendedInfo, new MatchInfo(response));
		}

		public Coroutine DestroyMatch(NetworkID netId, int requestDomain, BasicResponseDelegate callback)
		{
			return DestroyMatch(new DestroyMatchRequest
			{
				networkId = netId,
				domain = requestDomain
			}, callback);
		}

		internal Coroutine DestroyMatch(DestroyMatchRequest req, BasicResponseDelegate callback)
		{
			if (callback == null)
			{
				Debug.Log("callback supplied is null, aborting DestroyMatch Request.");
				return null;
			}
			Uri uri = new Uri(baseUri, "/json/reply/DestroyMatchRequest");
			Debug.Log("MatchMakingClient Destroy :" + uri.ToString());
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("version", Request.currentVersion);
			wWWForm.AddField("projectId", Application.cloudProjectId);
			wWWForm.AddField("sourceId", Utility.GetSourceID().ToString());
			wWWForm.AddField("accessTokenString", Utility.GetAccessTokenForNetwork(req.networkId).GetByteString());
			wWWForm.AddField("domain", req.domain);
			wWWForm.AddField("networkId", req.networkId.ToString());
			wWWForm.headers["Accept"] = "application/json";
			WWW client = new WWW(uri.ToString(), wWWForm);
			return StartCoroutine(ProcessMatchResponse<BasicResponse, BasicResponseDelegate>(client, OnMatchDestroyed, callback));
		}

		internal void OnMatchDestroyed(BasicResponse response, BasicResponseDelegate userCallback)
		{
			userCallback(response.success, response.extendedInfo);
		}

		public Coroutine DropConnection(NetworkID netId, NodeID dropNodeId, int requestDomain, BasicResponseDelegate callback)
		{
			return DropConnection(new DropConnectionRequest
			{
				networkId = netId,
				nodeId = dropNodeId,
				domain = requestDomain
			}, callback);
		}

		internal Coroutine DropConnection(DropConnectionRequest req, BasicResponseDelegate callback)
		{
			if (callback == null)
			{
				Debug.Log("callback supplied is null, aborting DropConnection Request.");
				return null;
			}
			Uri uri = new Uri(baseUri, "/json/reply/DropConnectionRequest");
			Debug.Log("MatchMakingClient DropConnection :" + uri);
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("version", Request.currentVersion);
			wWWForm.AddField("projectId", Application.cloudProjectId);
			wWWForm.AddField("sourceId", Utility.GetSourceID().ToString());
			wWWForm.AddField("accessTokenString", Utility.GetAccessTokenForNetwork(req.networkId).GetByteString());
			wWWForm.AddField("domain", req.domain);
			wWWForm.AddField("networkId", req.networkId.ToString());
			wWWForm.AddField("nodeId", req.nodeId.ToString());
			wWWForm.headers["Accept"] = "application/json";
			WWW client = new WWW(uri.ToString(), wWWForm);
			return StartCoroutine(ProcessMatchResponse<DropConnectionResponse, BasicResponseDelegate>(client, OnDropConnection, callback));
		}

		internal void OnDropConnection(DropConnectionResponse response, BasicResponseDelegate userCallback)
		{
			userCallback(response.success, response.extendedInfo);
		}

		public Coroutine ListMatches(int startPageNumber, int resultPageSize, string matchNameFilter, bool filterOutPrivateMatchesFromResults, int eloScoreTarget, int requestDomain, DataResponseDelegate<List<MatchInfoSnapshot>> callback)
		{
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				Debug.LogError("Matchmaking is not supported on WebGL player.");
				return null;
			}
			return ListMatches(new ListMatchRequest
			{
				pageNum = startPageNumber,
				pageSize = resultPageSize,
				nameFilter = matchNameFilter,
				filterOutPrivateMatches = filterOutPrivateMatchesFromResults,
				eloScore = eloScoreTarget,
				domain = requestDomain
			}, callback);
		}

		internal Coroutine ListMatches(ListMatchRequest req, DataResponseDelegate<List<MatchInfoSnapshot>> callback)
		{
			if (callback == null)
			{
				Debug.Log("callback supplied is null, aborting ListMatch Request.");
				return null;
			}
			Uri uri = new Uri(baseUri, "/json/reply/ListMatchRequest");
			Debug.Log("MatchMakingClient ListMatches :" + uri);
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("version", Request.currentVersion);
			wWWForm.AddField("projectId", Application.cloudProjectId);
			wWWForm.AddField("sourceId", Utility.GetSourceID().ToString());
			wWWForm.AddField("accessTokenString", 0);
			wWWForm.AddField("domain", req.domain);
			wWWForm.AddField("pageSize", req.pageSize);
			wWWForm.AddField("pageNum", req.pageNum);
			wWWForm.AddField("nameFilter", req.nameFilter);
			wWWForm.AddField("filterOutPrivateMatches", req.filterOutPrivateMatches.ToString());
			wWWForm.AddField("eloScore", req.eloScore.ToString());
			wWWForm.headers["Accept"] = "application/json";
			WWW client = new WWW(uri.ToString(), wWWForm);
			return StartCoroutine(ProcessMatchResponse<ListMatchResponse, DataResponseDelegate<List<MatchInfoSnapshot>>>(client, OnMatchList, callback));
		}

		internal void OnMatchList(ListMatchResponse response, DataResponseDelegate<List<MatchInfoSnapshot>> userCallback)
		{
			List<MatchInfoSnapshot> list = new List<MatchInfoSnapshot>();
			foreach (MatchDesc match in response.matches)
			{
				list.Add(new MatchInfoSnapshot(match));
			}
			userCallback(response.success, response.extendedInfo, list);
		}

		public Coroutine SetMatchAttributes(NetworkID networkId, bool isListed, int requestDomain, BasicResponseDelegate callback)
		{
			return SetMatchAttributes(new SetMatchAttributesRequest
			{
				networkId = networkId,
				isListed = isListed,
				domain = requestDomain
			}, callback);
		}

		internal Coroutine SetMatchAttributes(SetMatchAttributesRequest req, BasicResponseDelegate callback)
		{
			if (callback == null)
			{
				Debug.Log("callback supplied is null, aborting SetMatchAttributes Request.");
				return null;
			}
			Uri uri = new Uri(baseUri, "/json/reply/SetMatchAttributesRequest");
			Debug.Log("MatchMakingClient SetMatchAttributes :" + uri);
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("version", Request.currentVersion);
			wWWForm.AddField("projectId", Application.cloudProjectId);
			wWWForm.AddField("sourceId", Utility.GetSourceID().ToString());
			wWWForm.AddField("accessTokenString", Utility.GetAccessTokenForNetwork(req.networkId).GetByteString());
			wWWForm.AddField("domain", req.domain);
			wWWForm.AddField("networkId", req.networkId.ToString());
			wWWForm.AddField("isListed", req.isListed.ToString());
			wWWForm.headers["Accept"] = "application/json";
			WWW client = new WWW(uri.ToString(), wWWForm);
			return StartCoroutine(ProcessMatchResponse<BasicResponse, BasicResponseDelegate>(client, OnSetMatchAttributes, callback));
		}

		internal void OnSetMatchAttributes(BasicResponse response, BasicResponseDelegate userCallback)
		{
			userCallback(response.success, response.extendedInfo);
		}

		private IEnumerator ProcessMatchResponse<JSONRESPONSE, USERRESPONSEDELEGATETYPE>(WWW client, InternalResponseDelegate<JSONRESPONSE, USERRESPONSEDELEGATETYPE> internalCallback, USERRESPONSEDELEGATETYPE userCallback) where JSONRESPONSE : Response, new()
		{
			yield return client;
			JSONRESPONSE jsonInterface = new JSONRESPONSE();
			if (string.IsNullOrEmpty(client.error))
			{
				if (SimpleJson.SimpleJson.TryDeserializeObject(client.text, out object obj))
				{
					IDictionary<string, object> dictionary = obj as IDictionary<string, object>;
					if (dictionary != null)
					{
						try
						{
							jsonInterface.Parse(obj);
						}
						catch (FormatException ex)
						{
							jsonInterface.SetFailure(UnityString.Format("FormatException:[{0}] ", ex.ToString()));
						}
					}
				}
			}
			else
			{
				jsonInterface.SetFailure(UnityString.Format("Request error:[{0}] Raw response:[{1}]", client.error, client.text));
			}
			client.Dispose();
			internalCallback(jsonInterface, userCallback);
		}
	}
}
