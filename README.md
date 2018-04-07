# 프로젝트 디스코드 봇

![](https://postfiles.pstatic.net/MjAxODA0MDdfMjkg/MDAxNTIzMDY3MDY4MzI5.umgtC2W-YAvVXt9PqJL3rGTK9t1g62nz32-uhd3HqFYg.rXvYn5xmRqOw18JKYwvzHM3AQ6tWXHt9GmYDCX6WMpsg.JPEG.strix13/bandicam_2018-04-07_11-08-13-107.jpg?type=w773)
![](https://postfiles.pstatic.net/MjAxODA0MDdfMTUw/MDAxNTIzMDY3MDY4Mjc5.0fzYal5WPIRToIQ_7-GufYQ66bIf5fMPpoB0Bxk-AyEg.Mn1CAVUN8Hft7iR3JIEYojD0bkrnndkCBaxpYGrXFyQg.JPEG.strix13/bandicam_2018-04-07_11-08-21-189.jpg?type=w773)
![](https://postfiles.pstatic.net/MjAxODA0MDdfNDcg/MDAxNTIzMDY3MDY4MzMy.eD7x0HkwKDhEH_0_yq6K-TE_3H20aL04hhiH86V0mukg.-siBNBJbGpDK7ZHYqugRhHBOq66-X3JH69H7vaDwPJMg.JPEG.strix13/bandicam_2018-04-07_11-08-27-186.jpg?type=w773)
![](https://postfiles.pstatic.net/MjAxODA0MDdfMTg3/MDAxNTIzMDY3MDY4MzE1.K9vjgYYe_8L9Z5VctnBVMa6M4QAjQvZ2coGRxdRGS6Ug.Bb5Bcnj3-hK1oo6AnzsPjlLyOTJR844ou8pfrjXohbcg.JPEG.strix13/bandicam_2018-04-07_11-08-37-895.jpg?type=w773)


- 채팅 프로그램 디스코드에서 사용자 편의를 위한 봇을 제작하였습니다.
- 제작한 봇 리스트 : 검색 도우미, 방 관리자, 신문 배달부, 퀴즈봇
- 개발 툴 : C#, APM, NotePad++

## 0. 목차
0. 목차
1. 프로젝트 소개
2. 프로젝트 주요 기능 목록
    - 검색 도우미
    - 방관리자
    - 신문배달부
    - 퀴즈봇

---
## 1. 프로젝트 소개
  - 우주에서 다양한 종족의 캐릭터가 괴수를 물리치고 올림픽을 참가하는 게임입니다.
  - 슈팅 게임을 통해 미니게임 티켓을 획득하고, 미니게임은 시즌제로 랭킹에 따라 보상받는 게임입니다.
  - 캐릭터 구입 및 강화, 우주선 강화 시스템이 있습니다.
  - 슈팅게임의 경우 파츠 시스템이 있어서 파츠 장착 및 강화를 할 수 있습니다.
  - 쿠폰제를 통해 사전예약을 접수하였고, PHP와 DB를 통해 쿠폰 시스템을 구현하였습니다.
    - Winform을 통해 쿠폰 생성기를 제작하였습니다.

## 2. 프로젝트 주요 기능 목록
### 검색 도우미
- 검색 도우미의 경우 네이버 검색, 사전 검색, Unity Script 검색 총 3가지 기능을 가지고 있습니다.
- 채팅창에 검색을 명령하면 Chrome 크롤링으로 검색한 결과를 출력합니다.
```csharp
[Command("유니티")]
public async Task SearchStart_Unity_Kor(CommandContext pContext, string strSearchWord)
{
    if (CBot.CheckIsRespond(pContext.Channel) == false) return;

    await Event_SearchStart(pContext, strSearchWord, Search_UnityAPI, string.Format(strURL_UnityScriptAPI, pContext.RawArgumentString.Replace(" ", "+")), "UnityAPI", true);
}

static DiscordEmbedBuilder Search_UnityAPI( string strURL, string strSearchWord, DateTime pDateTimeStart )
{
IWebDriver pDriver = new ChromeDriver();
pDriver.Url = strURL;

// 본래 이렇게 해야 정상이지만, 빠른 속도를 위해 주석처리
// pDriver.Url = strURL_UnityScriptAPI;
// pDriver.Manage().Window.Maximize();
// IWebElement pElement_SearchInput = pDriver.FindElement( By.Id( "q" ) );
// pElement_SearchInput.SendKeys( strSearchWord );
// pDriver.FindElement( By.ClassName( "submit" ) ).Click();

DiscordEmbedBuilder pEmbedBuilder = new DiscordEmbedBuilder();
IWebElement pElement_ResultList = pDriver.FindElement( By.ClassName( "search-results" ) );
try
{
    IWebElement pElement_ListFirst = pElement_ResultList.FindElement( By.ClassName( "result" ) );
    IWebElement pElement_ChildNodeHref = pElement_ListFirst.FindElement( By.ClassName( "title" ) );
    string strResultURL = pElement_ChildNodeHref.GetAttribute( "href" );
    IWebElement pElement_ChildNode_Description = pElement_ListFirst.FindElement( By.TagName( "p" ) );

    ProcWrite_Result_Success( pEmbedBuilder, pDateTimeStart, pElement_ChildNodeHref.Text, strSearchWord, pElement_ChildNode_Description.Text, strResultURL, strURL );
}
catch
{
    ProcWrite_Result_Fail( pEmbedBuilder, strSearchWord );
}

    pDriver.Close();
    return pEmbedBuilder;
}
```

### 방관리자
- 방관리자의 경우 현재 새로 오신분들을 위한 Welcome 메세지 기능밖에 안되있습니다.
- 규모가 커지면 역할 부여, 매니져 관리 등을 할 예정이였습니다.

### 신문배달부
- 특정 시간에 특정 사이트에서 크롤링한 데이터를 채팅에 출력합니다.
- 현재 네이버 카페, 게임 뉴스(게임메카, 디스이즈게임즈), 유니티 블로그 등을 아침마다 크롤링해옵니다.
- 하단은 작성한 코드 일부 입니다.
```csharp
static private async Task ProcCrawling_UnityBlog(DiscordChannel pChannel, IWebElement pElement_Parents)
{
    if (pElement_Parents == null)
        return;

    IWebElement pElement_TitleParents = pElement_Parents.FindElement(By.TagName("h4"));
    IWebElement pElement_Title = pElement_TitleParents.FindElement(By.TagName("a"));

    IWebElement pElement_ImageParents = pElement_Parents.FindElement(By.TagName("a"));
    IWebElement pElement_Image = pElement_ImageParents.FindElement(By.TagName("div"));

    string strImageURL = pElement_Image.GetAttribute("style");
    int iCutString_StartIndex = strImageURL.IndexOf("https");
    int iCutString_FinishIndex = strImageURL.IndexOf(")");
    strImageURL = strImageURL.Substring(iCutString_StartIndex, iCutString_FinishIndex - iCutString_StartIndex - 1);

    IWebElement pElement_AuthorAndDate = pElement_Parents.FindElement(By.TagName("span"));
    IWebElement pElement_Author = pElement_AuthorAndDate.FindElement(By.TagName("a"));
    IWebElement pElement_Date = pElement_AuthorAndDate.FindElement(By.ClassName("c-mg"));

    string strDate = pElement_Date.Text;
    strDate = strDate.Replace("월", ",");

    IWebElement pElement_Contents = pElement_Parents.FindElement(By.TagName("p"));
    string strContents = pElement_Contents.Text;

    string strCategory = "";
    var arrElement_TagNameIs_Div = pElement_Parents.FindElements(By.TagName("a"));
    foreach(IWebElement pElement in arrElement_TagNameIs_Div)
    {
        try
        {
            if (pElement.GetAttribute("class").StartsWith("category-tag"))
            {
                strCategory = pElement.Text;
                break;
            }
        }
        catch
        {
            continue;
        }
    }

    if (Strix.CBot.CheckIsOverDay(strDate, 1) == false)
    {
        if (string.IsNullOrWhiteSpace(strContents))
            strContents = pElement_ImageParents.GetAttribute("href");

    strContents = strContents.Replace("더 읽기",
            $"[더 읽기]({pElement_ImageParents.GetAttribute("href")})");

        DiscordEmbedBuilder pEmbed = new DiscordEmbedBuilder();
        pEmbed.
            WithTitle("유니티 블로그 최신 글입니다.").
            WithUrl(const_UnityBlog).
            WithImageUrl(strImageURL).
            AddField(pElement_Title.Text, strContents).
            WithFooter($"{pElement_Author.Text} {pElement_Date.Text} ({strCategory})");

        await pChannel.SendMessageAsync(null, false, pEmbed);
    }
}
```

### 퀴즈봇
- 유저가 퀴즈를 등록하면, 관리자가 퀴즈 후보를 추려 퀴즈를 작성합니다.
- 퀴즈를 맞추면 포인트를 획득하며, 포인트에 따라 역할을 부여합니다.
- 퀴즈와 유저 정보는 DB에 기록하였습니다.
- 현재 핵심 기능은 구현하였습니다. ( 퀴즈 추가, 퀴즈 등록, 퀴즈 삭제, 포인트, 역할 부여 등)
- [핵심 코드 링크](https://github.com/KorStrix/DiscordBot_Unity/blob/master/Bot_Quiz/Command_Quiz.cs)
