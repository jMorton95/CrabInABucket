namespace FinanceManager.Core.Responses;

public record FriendListResponse
(
    List<NamedUserResponse> Friends,
    List<NamedUserResponse> PendingFriends,
    List<NamedUserResponse> SuggestedFriends,
    List<NamedUserResponse> RandomSuggestions
);