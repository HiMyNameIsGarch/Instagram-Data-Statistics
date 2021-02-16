namespace Instagram_Data_Statistics.Enums
{
    public enum DataType
    {
        Likes,//comment and media
        Account_History,//login history, registration info
        Comments,
        Media,//includes all the files that you send(stories, profile,photos,videos, direct)
        Connections,//all blocked users, follow request send, permanent follow request, followers, following, hastags, dimissed suggested user
        Messages,
        Saved,//collections and media
        Searched_Content,//all accounts that you searched
        Seen_Content,//all the posts that you saw
        Stories//polls,emoji slider, questions, countdowns,quizzes
    }
}
